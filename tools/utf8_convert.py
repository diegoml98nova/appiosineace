"""Convierte los .cs a UTF-8 con BOM (RNF-MANT-002, AD-14, [EP1]-01) y verifica la codificacion.

Uso:
  python tools/utf8_convert.py verificar <raiz>      solo lee; exit 1 si hay .cs sin BOM o no UTF-8, U+FFFD o mojibake
  python tools/utf8_convert.py convertir <raiz>      reescribe los .cs: cp1252 -> UTF-8; agrega BOM si falta

Reglas: un archivo que ya decodifica como UTF-8 solo recibe BOM si no lo tiene (el texto no cambia).
Un archivo que no decodifica como UTF-8 se decodifica como Windows-1252 (SPK-08: sin bytes sin asignar ni mezcla);
si tuviera bytes sin asignar en cp1252, se detiene sin escribir. Conserva los saltos de linea del original.
"""
import os
import re
import sys

BOM = b"\xef\xbb\xbf"
UNDEF = {0x81, 0x8D, 0x8F, 0x90, 0x9D}
MOJI = re.compile("Ã[\u0080-¿]|Â[\u0080-¿]|â€")


def cs_files(raiz):
    for r, d, fs in os.walk(raiz):
        d[:] = [x for x in d if x not in ("bin", "obj", ".git", ".vs")]
        for f in fs:
            if f.endswith(".cs"):
                yield os.path.join(r, f)


def clasificar(path):
    b = open(path, "rb").read()
    bom = b.startswith(BOM)
    body = b[3:] if bom else b
    try:
        s = body.decode("utf-8")
        return b, "utf8", bom, s
    except UnicodeDecodeError:
        pass
    if any(x in UNDEF for x in body if x >= 0x80):
        return b, "cp1252-no-asignado", bom, None
    return b, "cp1252", bom, body.decode("cp1252")


def main():
    modo, raiz = sys.argv[1], sys.argv[2]
    n = nbom = ncp = 0
    problemas = []
    for p in sorted(cs_files(raiz)):
        n += 1
        b, tipo, bom, s = clasificar(p)
        rel = os.path.relpath(p, raiz)
        if tipo == "cp1252-no-asignado":
            problemas.append((rel, "bytes sin asignar en cp1252"))
            continue
        if modo == "convertir":
            if tipo == "cp1252":
                ncp += 1
            if tipo == "cp1252" or not bom:
                open(p, "wb").write(BOM + s.encode("utf-8"))
                if tipo == "utf8":
                    nbom += 1
        else:
            if tipo == "cp1252":
                ncp += 1
                problemas.append((rel, "no es UTF-8 (cp1252)"))
            elif not bom:
                nbom += 1
                problemas.append((rel, "UTF-8 sin BOM"))
            if "�" in s:
                problemas.append((rel, "contiene U+FFFD"))
            if MOJI.search(s):
                problemas.append((rel, "posible mojibake"))
    print("archivos .cs: %d | cp1252 %s: %d | UTF-8 sin BOM %s: %d" % (
        n, "convertidos" if modo == "convertir" else "pendientes", ncp,
        "con BOM agregado" if modo == "convertir" else "pendientes", nbom))
    for rel, msg in problemas:
        print("  PROBLEMA:", rel, "-", msg)
    sys.exit(1 if problemas else 0)


main()
