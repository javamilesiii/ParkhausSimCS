namespace ParkhausUI.Models
{
    public class Barrier
    {
        public string Name { get; set; }
        public bool IsOpen { get; private set; }

        public Barrier(string name)
        {
            this.Name = name;
            this.IsOpen = false;
        }

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