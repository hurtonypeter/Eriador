create procedure GetCurrentPermissions
	@UserId nvarchar(128)
as

select [Permission].[Id], [Permission].[MachineReadableName], [Permission].[ModuleId], [Permission].[Name]
from [Permission]
join [AspNetRolePermissions] on [Permission].[Id] = [AspNetRolePermissions].[PermissionId]
join [AspNetRoles] on [AspNetRolePermissions].[RoleId] = [AspNetRoles].[Id]
join [AspNetUserRoles] on [AspNetRoles].[Id] = [AspNetUserRoles].[RoleId]
join [AspNetUsers] on [AspNetUserRoles].[UserId] = [AspNetUsers].[Id]
where [AspNetUsers].[Id] = @UserId