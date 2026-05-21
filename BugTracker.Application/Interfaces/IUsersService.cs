using BugTracker.Application.DTOs;

namespace BugTracker.Application.Interfaces;

public interface IUsersService
{
    public Task<ResponseWrapper<UserDto>> CreateAsync(UserDto userDto);
    public Task<ResponseWrapper<List<UserDto>>> ReadAllAsync();
    public Task<ResponseWrapper<UserDto>> ReadByIdAsync(int id);
    public Task<ResponseWrapper<UserDto>> UpdateAsync(UserDto userPatch, int id);
    public Task<ResponseWrapper<int>> DeleteAsync(int id);
}