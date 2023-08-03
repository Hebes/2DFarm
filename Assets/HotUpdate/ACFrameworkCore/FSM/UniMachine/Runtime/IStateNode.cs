
namespace ACFrameworkCore
{
	public interface IStateNode
	{
		void OnCreate(CStateMachine machine);
		void OnEnter();
		void OnUpdate();
		void OnExit();
	}
}