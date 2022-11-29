using API_SmartTicket.DTos.Bot;
using API_SmartTicket.Repository;
using Bot.DTos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Functions
{
    public class FuncionesBot
    {
        public static async Task<string> GetTokenSmartAsync(string domain)
        {
            using (var client = new HttpClient())
            {
                var auth = new AuthSmart();
                auth.username = "test1";
                auth.password = "password1";
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(auth);
                var data = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{domain}/api/Authorization/Authenticate";

                var response = await client.PostAsync(url, data);

                string token = response.Content.ReadAsStringAsync().Result;

                return token;
            }
        }

        public static async Task<List<EventoActivoDTo>> GetListaEventosBotAsync(string domain, string token, string pais)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    return await client.GetFromJsonAsync<List<EventoActivoDTo>>($"{domain}/api/Bot/Eventos/{pais}");

                }catch(Exception ex)
                {
                    return null;
                }

                
            }
        }

        public static async Task<EventoActivoDTo> GetEventoBotAsync(string domain, string token, string evento)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var eventoDTo = new RequestBodyEventoDTo() { evento = evento};
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(eventoDTo);
                    var data = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync($"{domain}/api/Bot/Eventos/Evento", data);

                    return await response.Content.ReadFromJsonAsync<EventoActivoDTo>();
                }
                catch (Exception ex)
                {
                    return null;
                }


            }
        }

        public static async Task<List<FechaEventoDTo>> GetListaFechasBotAsync(string domain, string token, string evento)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var eventoDTo = new RequestBodyEventoDTo() { evento = evento };
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(eventoDTo);
                    var data = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync($"{domain}/api/Bot/Eventos/Fechas", data);

                    return await response.Content.ReadFromJsonAsync<List<FechaEventoDTo>>();
                }
                catch (Exception ex)
                {
                    return null;
                }


            }
        }

        public static async Task<List<PrecioEventoDTo>> GetListaPreciosBotAsync(string domain, string token, string evento)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var eventoDTo = new RequestBodyEventoDTo() { evento = evento };
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(eventoDTo);
                    var data = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync($"{domain}/api/Bot/Eventos/Precios", data);

                    return await response.Content.ReadFromJsonAsync<List<PrecioEventoDTo>>();
                }
                catch (Exception ex)
                {
                    return null;
                }


            }
        }
    }
}
