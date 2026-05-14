namespace PrimePOS.Contracts.DTOs.Barcode
{
    public class BarcodeDto
    {
        public string Codigo { get; set; } = "";

        public byte[] Imagen { get; set; } = Array.Empty<byte>();
    }
}
