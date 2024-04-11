using System.Text;
using System.Text.Json;
using TheBookOfAnswerAPI.Models;

namespace TheBookOfAnswerAPI.Service
{
    public static class AnswerService{
        public static string firebaseDatabaseUrl = "https://the-book-of-answers-a0421-default-rtdb.asia-southeast1.firebasedatabase.app/";
        public static string firebaseDatabaseDocument = "Answers";
        static readonly HttpClient client = new HttpClient();

        static List<Answer>? ListAnswers {get; set;}

        public static async Task<List<Answer>?> GetAll(){
            string url = $"{firebaseDatabaseUrl}" +
                        $"{firebaseDatabaseDocument}.json";

            var httpResponseMessage = await client.GetAsync(url);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
                
                if (contentStream != null && contentStream != "null")
                {
                    for(int i = 0; i < contentStream.Count(); i++){
                        ListAnswers = JsonSerializer.Deserialize<List<Answer>>(contentStream);
                    }
                }
            }

            return ListAnswers;
        }

        public static async Task<Answer?> GetById(int id){
            string url = $"{firebaseDatabaseUrl}" +
                        $"{firebaseDatabaseDocument}/" +
                        $"{id}.json";
            
            var httpResponseMessage = await client.GetAsync(url);

            Answer answer = new Answer();

            if(httpResponseMessage.IsSuccessStatusCode){
                var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();

                if(contentStream != null && contentStream != "null"){
                    var result = JsonSerializer.Deserialize<Answer>(contentStream);

                    if(result is null){
                        return null;
                    }

                    answer.Id = result.Id;
                    answer.English = result.English;
                    answer.Vietnamese = result.Vietnamese;
                }
            }

            return answer;
        }

        public static async Task<Answer?> Add(Answer answer){
            // answer.Id = Guid.NewGuid().ToString("N");

            string answerJsonString = JsonSerializer.Serialize(answer);

            var payload = new StringContent(answerJsonString, Encoding.UTF8, "application/json");

            string url = $"{firebaseDatabaseUrl}" +
                        $"{firebaseDatabaseDocument}/" +
                        $"{answer.Id}.json";

            var httpResponseMessage = await client.PutAsJsonAsync(url, payload);

            if(httpResponseMessage.IsSuccessStatusCode){
                var contentStream = await httpResponseMessage.Content.ReadAsByteArrayAsync();
                var result = JsonSerializer.Deserialize<Answer>(contentStream);

                return result;
            }

            return null;
        }
       
        public static async Task<string?> DeleteById(int id)
        {
            string url = $"{firebaseDatabaseUrl}" +
                        $"{firebaseDatabaseDocument}/" +
                        $"{id}.json";

            var httpResponseMessage = await client.DeleteAsync(url);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
                if(contentStream == "null")
                {
                    return "Deleted";
                }
            }

            return null;
        }

        public static async Task<Answer?> Update(Answer answer, int id)
        {
            answer.Id = id;
            string answerJsonString = JsonSerializer.Serialize(answer);

            var payload = new StringContent(answerJsonString, Encoding.UTF8, "application/json");

            string url = $"{firebaseDatabaseUrl}" +
                        $"{firebaseDatabaseDocument}/" +
                        $"{id}.json";


            var httpResponseMessage = await client.PutAsync(url, payload);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();

                if (contentStream != null && contentStream != "null")
                {
                    var result = JsonSerializer.Deserialize<Answer>(contentStream);

                    return result;
                }         
            }

            return null;
        }
    }
}