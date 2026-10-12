using SineaceAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace SineaceAPP.Services
{
    public class SuneduService
    {
        public string url { get; set; }
        public SuneduService()
        {
            url = "https://appsineace2-servicioapp.azurewebsites.net/api/appSunedu/";
        }
        public UniversidadModel GetUniversidades()
        {
            HttpClient client = new HttpClient();
            UniversidadModel Items = new UniversidadModel();
            JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            Uri uri = new Uri(string.Format(url+"GetUniversidades", string.Empty));
            try
            {
                string json = $"{{\"entidadTipo\":\"\",\"entidadNombre\":\"\",\"entidadTipoGestion\":\"\",\"NombrePrograma\":\"\",\"esLocalPrincipal\":\"\"}}";
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(uri, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string content2 = response.Content.ReadAsStringAsync().Result;
                    Items = JsonSerializer.Deserialize<UniversidadModel>(content2, _serializerOptions);
                }
                return Items;
            } catch (Exception ex)
            {
                return Items;
            }
        }
        public SuneduModel GetProgramas(string Institucion, string filtro)
        {
            HttpClient client = new HttpClient();
            SuneduModel Items = new SuneduModel();
            JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            Uri uri = new Uri(string.Format(url + "GetSunedu", string.Empty));
            try
            {
                string json = $"{{\"entidadNombre\":\"\",\"programaModalidadEstudio\":\""+filtro+"\",\"programaNombreEstandar\":\""+Institucion+"\",\"programaTipoNivelAcademico\":\"\",}}";
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(uri, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string content2 = response.Content.ReadAsStringAsync().Result;
                    Items = JsonSerializer.Deserialize<SuneduModel>(content2, _serializerOptions);
                }
                return Items;
            } catch (Exception ex)
            {
                return Items;
            }
        }
        public async Task<SuneduModel> GetProgramas2(string Institucion, string filtro)
        {
            string apiUrl = string.Format(url + "GetSunedu", string.Empty);
            SuneduModel Items = new SuneduModel();
            JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            // String parameter with quotation marks
            string parameterValue1 = "";
            string parameterValue2 = filtro;
            string parameterValue3 = Institucion;
            string parameterValue4 = "";

            // Create an instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Prepare the content to be sent in the request body
                string jsonPayload = $"{{ \"parameter\": \"{parameterValue1}\", \"programaModalidadEstudio\":\"{parameterValue2}\", \"programaNombreEstandar\":\"{parameterValue3}\", \"programaTipoNivelAcademico\":\"{parameterValue4}\"}}";
                StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Send a POST request with the payload
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                // Check the response status
                if (response.IsSuccessStatusCode)
                {
                    // Process the response
                    string result = await response.Content.ReadAsStringAsync();
                    Items = JsonSerializer.Deserialize<SuneduModel>(result, _serializerOptions);
                } 
            }
            return Items;

        }
    }
}
