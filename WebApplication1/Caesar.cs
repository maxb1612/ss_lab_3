using System.Text;

namespace security;

public static class Caesar
{
    private static char Cipher(char ch, int key) {  
        if (!char.IsLetter(ch)) {  
  
            return ch;  
        }

        var offset = char.IsUpper(ch) ? 'A' : 'a';
        
        return (char)((((ch + key) - offset) % 26) + offset);
    }  
  
  
    public static string Encipher(string input, int key)
    {
        StringBuilder bld = new StringBuilder();

        foreach(char ch in input)  
            bld.Append(Cipher(ch, key));

        string output = bld.ToString();
        return output;  
    }  
  
    public static string Decipher(string input, int key) {  
        return Encipher(input, 26 - key);  
    }
}