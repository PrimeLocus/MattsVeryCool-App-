using System.ComponentModel.DataAnnotations.Schema;

namespace MattsVeryCoolApp.Models;

public class Shipment
{
    public int ShipmentId { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public double Weight { get; set; }
    public string Status { get; set; }
    public int CustomerId { get; set; }
    [ForeignKey("CustomerId")]
    public virtual Customer? Customer { get; set; }
    public int? DriverId { get; set; }
    public virtual Driver? Driver { get; set; }
}