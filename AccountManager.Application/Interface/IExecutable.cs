namespace AccountManager.Application.Interface;

public interface IExecutable<in TParameter, out TValue>
{
    TValue Execute(TParameter data);
}