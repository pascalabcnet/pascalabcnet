
namespace Mono.Debugger.Soft
{
	public class MethodUpdateEvent : Event
	{
		long id;
		MethodMirror method;
		internal MethodUpdateEvent (VirtualMachine vm, int req_id, long thread_id, long id) : base (EventType.MethodUpdate, vm, req_id, thread_id)
		{
			this.id = id;
			method = vm.GetMethod (id);
		}

		public MethodMirror GetMethod()
		{
			return method;
		}
	}
}
