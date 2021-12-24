namespace Gamefreak130.Common.Tasks
{
    using Gamefreak130.Common.Loggers;
    using Sims3.SimIFace;
    using System;

    public abstract class CommonTask : Task
    {
        public bool IsStopped => ObjectId.IsValid;

        public override void Dispose()
        {
            if (ObjectId != ObjectGuid.InvalidObjectGuid)
            {
                Simulator.DestroyObject(ObjectId);
                ObjectId = ObjectGuid.InvalidObjectGuid;
            }
            base.Dispose();
        }

        protected abstract void Perform();

        public override void Simulate()
        {
            try
            {
                Perform();
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
                try
                {
                    Dispose();
                }
                catch (ResetException)
                {
                }
                catch (Exception ex)
                {
                    ExceptionLogger.sInstance.Log(ex);
                }
            }
        }

        public override void Stop() => Dispose();

        public virtual ObjectGuid Start() => Simulator.AddObject(this);

        public override string ToString() 
            => $"{base.ToString()}, ObjectId: {ObjectId}";
    }
}
