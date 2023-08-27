namespace MouseAndCreate.Rendering
{
    public interface IShaderSource
    {
        string Name { get; }
        ShaderSourceType Type { get; }
        string GetContent();
    }
}
