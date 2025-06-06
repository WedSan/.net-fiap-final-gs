namespace AlertNotificationService;
public class LocationExtractor
    {
        public static (string city, string neighborhood, string state)? ExtractLocationInfo(string message)
        {
            try
            {
                
                string city = null;
                string neighborhood = null;
                string state = null;
                
                if (message.Contains("São Paulo"))
                {
                    city = "São Paulo";
                    state = "SP";
                    if (message.Contains("Centro")) neighborhood = "Centro";
                }
                else if (message.Contains("Rio de Janeiro"))
                {
                    city = "Rio de Janeiro";
                    state = "RJ";
                    if (message.Contains("Copacabana")) neighborhood = "Copacabana";
                }
                else if (message.Contains("Belo Horizonte"))
                {
                    city = "Belo Horizonte";
                    state = "MG";
                    if (message.Contains("Savassi")) neighborhood = "Savassi";
                }
                else if (message.Contains("Curitiba"))
                {
                    city = "Curitiba";
                    state = "PR";
                    if (message.Contains("Centro")) neighborhood = "Centro";
                }
                else if (message.Contains("Recife"))
                {
                    city = "Recife";
                    state = "PE";
                    if (message.Contains("costeira")) neighborhood = "Boa Vista";
                }
                
                if (city != null && state != null)
                {
                    return (city, neighborhood ?? "", state);
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting location info: {ex.Message}");
                return null;
            }
        }
    }