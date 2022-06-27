namespace WebApplication1.DataAccess;

public class Captcha
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public int Num { get; set; }
    public string FilePath { get; set; }
}