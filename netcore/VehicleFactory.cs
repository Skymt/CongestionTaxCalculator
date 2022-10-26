namespace congestion.calculator
{
    public static class VehicleFactory
    {
        public static Vehicle GetVehicle(string vehicleType) => vehicleType switch
        {
            "Car" => new Car(),
            "Motorbike" => new Motorbike(),
            _ => new GenericVehicle { VehicleType = vehicleType }
        };

        class GenericVehicle : Vehicle
        {
            public string VehicleType { get; set; }

            public string GetVehicleType() => VehicleType;
        }
    }
}
