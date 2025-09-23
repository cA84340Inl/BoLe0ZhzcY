// 代码生成时间: 2025-09-23 13:09:46
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

// 定义权限枚举，便于管理不同的权限级别
public enum Permission {
    Read = 1,
    Write = 2,
    Edit = 4,
    Delete = 8
}

// 用户类，包含用户信息和权限
public class User {
    public string UserId { get; set; }
    public string Name { get; set; }
    public List<Permission> Permissions { get; set; } = new List<Permission>();
}

// 用户权限管理系统类
public class UserPermissionManagement : ComponentBase {
    [Inject]
    private IJSRuntime JSRuntime { get; set; }

    // 用户列表
    private List<User> users = new List<User>() {
        new User { UserId = "1", Name = "Alice", Permissions = new List<Permission> { Permission.Read, Permission.Edit } },
        new User { UserId = "2", Name = "Bob", Permissions = new List<Permission> { Permission.Read, Permission.Delete } }
    };

    // 添加用户的权限
    public void AddPermission(string userId, Permission permission) {
        var user = users.FirstOrDefault(u => u.UserId == userId);
        if (user != null) {
            if (!user.Permissions.Contains(permission)) {
                user.Permissions.Add(permission);
                Console.WriteLine($"Added {permission} permission to {user.Name}.");
            }
        } else {
            Console.WriteLine("User not found.");
        }
    }

    // 移除用户的权限
    public void RemovePermission(string userId, Permission permission) {
        var user = users.FirstOrDefault(u => u.UserId == userId);
        if (user != null) {
            if (user.Permissions.Contains(permission)) {
                user.Permissions.Remove(permission);
                Console.WriteLine($"Removed {permission} permission from {user.Name}.");
            }
        } else {
            Console.WriteLine("User not found.");
        }
    }

    // 检查用户是否具有特定的权限
    public bool HasPermission(string userId, Permission permission) {
        var user = users.FirstOrDefault(u => u.UserId == userId);
        return user != null && user.Permissions.Contains(permission);
    }

    // 用于从JS调用的方法，用于添加权限
    public async Task AddPermissionFromJS(string userId, string permissionName) {
        try {
            Permission permission = Enum.Parse<Permission>(permissionName);
            AddPermission(userId, permission);
            await JSRuntime.InvokeVoidAsync("alert", $"Added {permissionName} permission to user {userId}.");
        } catch (ArgumentException) {
            await JSRuntime.InvokeVoidAsync("alert", $"Invalid permission: {permissionName}.");
        }
    }

    // 用于从JS调用的方法，用于移除权限
    public async Task RemovePermissionFromJS(string userId, string permissionName) {
        try {
            Permission permission = Enum.Parse<Permission>(permissionName);
            RemovePermission(userId, permission);
            await JSRuntime.InvokeVoidAsync("alert", $"Removed {permissionName} permission from user {userId}.");
        } catch (ArgumentException) {
            await JSRuntime.InvokeVoidAsync("alert", $"Invalid permission: {permissionName}.");
        }
    }
}
