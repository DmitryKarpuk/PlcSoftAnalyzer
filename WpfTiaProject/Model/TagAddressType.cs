namespace WpfTiaProject.Model
{
    /// <summary>
    /// Represents different types of tag addresses in a PLC project.
    /// These types are used to categorize tags based on their memory location.
    /// </summary>
    public enum TagAddressType
    {
        Merker,
        Input,
        Output,
        Timer,
        Undefined
    }
}