namespace RealWordBE.Authentication.Logout
{
    public interface ITokenManager
    {
        void DeactivateAsync(string token);
        void DeactivateCurrentAsync();
        bool IsActiveAsync(string token);
        bool IsCurrentActiveToken();
    }
}
