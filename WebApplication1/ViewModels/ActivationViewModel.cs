namespace WebApplication1.ViewModels;

public class ActivationViewModel
{
    public string ActivationKey { get; set; }
    public bool IsActivated { get; set; }
    
    public string CaptchaName { get; set; }
    public int CaptchaText { get; set; }
}