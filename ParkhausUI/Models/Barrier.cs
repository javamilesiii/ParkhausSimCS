namespace ParkhausUI.Models
{
    public class Barrier(string name)
    {
        public string Name { get; set; } = name;
        public bool IsOpen { get; private set; } = false;

        public async Task PassThroughBarrierAsync()
        {
            this.OpenBarrier();
            await Task.Delay(2000);
            this.CloseBarrier();
        }

        public virtual void OpenBarrier()
        {
            IsOpen = true;
        }

        public virtual void CloseBarrier()
        {
            IsOpen = false;
        }
    }
}