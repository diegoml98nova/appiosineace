"""Retira de los .cs los bloques condicionales de plataformas que ya no existen (CA-009.2, [EP1]-02).

Uso: python tools/quitar_directivas.py <raiz>
Simbolos falsos: ANDROID, MACCATALYST, WINDOWS, TIZEN. IOS y DEBUG se conservan.
Soporta '#if A || B', '#elif', '#else', '#endif' anidados. Conserva BOM y saltos de linea.
Una condicion distinta de una lista de simbolos separados por '||' detiene el proceso.
"""
import os
import re
import sys

FALSOS = {"ANDROID", "MACCATALYST", "WINDOWS", "TIZEN"}
DIR = re.compile(r"^\s*#\s*(if|elif|else|endif)\b(.*)$")


def parse(lines, i=0, dentro=False):
    nodes = []
    while i < len(lines):
        l = lines[i]
        m = DIR.match(l.lstrip("\ufeff").rstrip("\r\n"))
        if m and m.group(1) == "if":
            ramas = []
            cond = m.group(2).strip()
            cuerpo, i, fin = parse(lines, i + 1, True)
            ramas.append((cond, cuerpo, l))
            otro = None
            while fin in ("elif", "else"):
                lin = lines[i - 1]
                if fin == "elif":
                    c2 = DIR.match(lin.rstrip("\r\n")).group(2).strip()
                    b2, i, fin = parse(lines, i, True)
                    ramas.append((c2, b2, lin))
                else:
                    b2, i, fin = parse(lines, i, True)
                    otro = (b2, lin)
            nodes.append(("if", ramas, otro, lines[i - 1]))
            continue
        if m and m.group(1) in ("elif", "else", "endif"):
            if not dentro:
                raise SystemExit("directiva sin #if: " + l)
            return nodes, i + 1, m.group(1)
        nodes.append(("t", l))
        i += 1
    return nodes, i, None


def plegar(cond):
    simb = [s.strip() for s in cond.split("||")]
    if not all(re.fullmatch(r"[A-Z_]+", s) for s in simb):
        raise SystemExit("condicion no soportada: " + cond)
    rest = [s for s in simb if s not in FALSOS]
    return " || ".join(rest) if rest else None


def emitir(nodes, out):
    for n in nodes:
        if n[0] == "t":
            out.append(n[1])
            continue
        _, ramas, otro, endl = n
        eol = "\r\n" if endl.endswith("\r\n") else "\n"
        vivas = [(plegar(c), b, l) for c, b, l in ramas]
        vivas = [(c, b, l) for c, b, l in vivas if c is not None]
        if not vivas:
            if otro:
                emitir(otro[0], out)
            continue
        for k, (c, b, l) in enumerate(vivas):
            kw = "#if" if k == 0 else "#elif"
            out.append("%s %s%s" % (kw, c, eol))
            emitir(b, out)
        if otro:
            out.append("#else" + eol)
            emitir(otro[0], out)
        out.append(endl if endl.lstrip().startswith("#endif") else "#endif" + eol)


def main():
    raiz = sys.argv[1]
    cambiados = 0
    for r, d, fs in os.walk(raiz):
        d[:] = [x for x in d if x not in ("bin", "obj", ".git", ".vs", "Platforms")]
        for f in fs:
            if not f.endswith(".cs"):
                continue
            p = os.path.join(r, f)
            raw = open(p, "rb").read()
            bom = raw.startswith(b"\xef\xbb\xbf")
            txt = raw[3:].decode("utf-8") if bom else raw.decode("utf-8")
            if not re.search(r"#\s*(if|elif)[^\n]*(ANDROID|MACCATALYST|WINDOWS|TIZEN)", txt):
                continue
            lines = txt.splitlines(True)
            nodes, _, _ = parse(lines)
            out = []
            emitir(nodes, out)
            nuevo = "".join(out)
            if nuevo != txt:
                open(p, "wb").write((b"\xef\xbb\xbf" if bom else b"") + nuevo.encode("utf-8"))
                cambiados += 1
                print("modificado:", os.path.relpath(p, raiz))
    print("archivos modificados:", cambiados)


main()
