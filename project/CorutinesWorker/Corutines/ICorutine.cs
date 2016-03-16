namespace CorutinesWorker.Corutines
{
    public interface ICorutine
    {
        /// <summary>
        /// Is corutine now active
        /// </summary>
        bool IsWorking { get; }

        bool HasStarted { get; }

        /// <summary>
        /// Cancel execution
        /// </summary>
        void Cancel();

        /// <summary>
        /// Start execution
        /// </summary>
        void Start();
    }
}