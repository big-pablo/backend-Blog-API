namespace Blog.API.Models.DTOs
{
    public class PageInfoDTO
    {
        public string Size { get; set; } //Кол-во постов на странице
        public string Count { get; set; } //Кол-во страниц
        public string Current { get; set; } //Номер страницы

    }
}
