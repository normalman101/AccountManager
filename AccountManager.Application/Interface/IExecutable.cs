using System.Threading.Tasks;
using AccountManager.Application.DTOs;

namespace AccountManager.Application.Interface;

public interface IExecutable<out TValue, in TParameter>
{
    TValue Execute(TParameter request);
}