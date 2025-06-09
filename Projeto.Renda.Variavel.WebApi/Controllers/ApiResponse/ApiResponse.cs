namespace Projeto.Renda.Variavel.WebApi.Controllers.ApiResponse
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
