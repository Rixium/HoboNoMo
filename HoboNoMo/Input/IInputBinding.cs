namespace HoboNoMo.Input
{
    public interface IInputBinding
    {
        bool Held { get; set; }
        bool Press { get; set; }
        
        bool Down { get; set; }

        void Update(float delta);
    }
}