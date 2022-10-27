using System.ComponentModel.DataAnnotations;

namespace SignatureServiceApi.ViewModels
{
    public class CheckPublicKeyRequest
    {
        [Required]
        public byte[] Key { get; set; }
    }
}
