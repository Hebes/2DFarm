
namespace Core
{
	public interface IStateNode
	{
		void OnCreate(StateMachineSystem machine);
		void OnEnter();
		void OnUpdate();
		void OnExit();
	}
}