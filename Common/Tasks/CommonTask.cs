namespace Gamefreak130.Common.Tasks
{
    using Gamefreak130.Common.Loggers;
    using Sims3.SimIFace;
    using System;

    public abstract class CommonTask : Task
    {
        public override void Dispose()
        {
            if (ObjectId != ObjectGuid.InvalidObjectGuid)
            {
                Simulator.DestroyObject(ObjectId);
                ObjectId = ObjectGuid.InvalidObjectGuid;
            }
            base.Dispose();
        }

        protected abstract void Run();

        public override void Simulate()
        {
            try
            {
                Run();
            }
            catch (ResetException)
            {
                throw;
            }
            catch (Exception ex)
            {
                ExceptionLogger.sInstance.Log(ex);
            }
            finally
            {
                Dispose();
            }
        }
    }
}
