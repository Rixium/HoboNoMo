namespace HoboNoMo.Input
{
    public interface IInputBinding
    {
        bool Held { get; set; }
        bool Press { get; set; }

        void Update(float delta);
    }
}