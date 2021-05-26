namespace BookStoreWebApp.Services
{
    public interface IUserService
    {
        public string GetUserId();
        public bool IsAuthenticated();
    }
}