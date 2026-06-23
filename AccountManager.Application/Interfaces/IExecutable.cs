namespace AccountManager.Application.Interfaces;

public interface IExecutable<out TValue, in TParameter>
{
    TValue Execute(TParameter request);
}