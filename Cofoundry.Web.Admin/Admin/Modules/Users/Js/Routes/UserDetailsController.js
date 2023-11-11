﻿angular.module('cms.users').controller('UserDetailsController', [
    '$routeParams',
    '$location',
    '$q',
    'shared.LoadState',
    'shared.modalDialogService',
    'shared.permissionValidationService',
    'shared.roleService',
    'shared.currentUser',
    'users.userService',
    'users.options',
function (
    $routeParams,
    $location,
    $q,
    LoadState,
    modalDialogService,
    permissionValidationService,
    roleService,
    currentUser,
    userService,
    options
    ) {

    var vm = this,
        isCurrentUser = currentUser.userId == $routeParams.id;

    init();
    
    /* INIT */

    function init() {

        // UI actions
        vm.edit = edit;
        vm.save = save;
        vm.cancel = reset;
        vm.resetPassword = resetPassword;
        vm.deleteUser = deleteUser;

        // Properties
        vm.editMode = false;
        vm.globalLoadState = new LoadState();
        vm.saveLoadState = new LoadState();
        vm.formLoadState = new LoadState(true);
        vm.userArea = options;

        vm.isCofoundryAdmin = options.userAreaCode === 'COF';
        vm.isCurrentUser = isCurrentUser;
        
        // Init
        $q.all([loadRoles(), loadUser()])
            .then(initForm)
            .then(setLoadingOff.bind(null, vm.formLoadState));
    }

    /* UI ACTIONS */

    function edit() {
        vm.editMode = true;
        vm.mainForm.formStatus.clear();
    }

    function save() {
        setLoadingOn(vm.saveLoadState);

        userService.update(vm.command)
            .then(onSuccess.bind(null, 'Changes were saved successfully'))
            .finally(setLoadingOff.bind(null, vm.saveLoadState));
    }

    function reset() {
        vm.editMode = false;
        vm.command = mapUpdateCommand();
        vm.mainForm.formStatus.clear();
    }

    function resetPassword() {
        var options = {
            title: 'Reset Password',
            message: 'Resetting a password will sign the user out of all sessions and email them a new temporary password that needs to be changed at first sign in.<br><br>Do you want to continue?',
            okButtonTitle: 'Yes, reset it',
            onOk: onOk
        };

        modalDialogService.confirm(options);

        function onOk() {
            setLoadingOn();

            return userService
                .resetPassword(vm.user.userId)
                .then(onSuccess.bind(null, 'Password reset, notification sent.'))
                .finally(setLoadingOff);
        }
    }

    function deleteUser() {
        var options = {
            title: 'Delete User',
            message: 'Are you sure you want to delete this user?',
            okButtonTitle: 'Yes, delete it',
            onOk: onOk
        };

        modalDialogService.confirm(options);

        function onOk() {
            setLoadingOn();

            return userService
                .remove(vm.user.userId)
                .then(redirectToList)
                .catch(setLoadingOff);
        }
    }

    /* PRIVATE FUNCS */

    function onSuccess(message) {
        return loadUser()
            .then(initForm)
            .then(vm.mainForm.formStatus.success.bind(null, message));
    }

    function loadRoles() {

        return roleService
            .getSelectionList(options.userAreaCode)
            .then(load);

        function load(result) {

            if (result) {
                vm.roles = result.items;
            }
        }

    }

    function loadUser() {
        var userId = $routeParams.id;

        return userService
            .getById(userId)
            .then(load);

        function load(user) {

            vm.user = user;

            setPermissions(user);
        }
    }

    function setPermissions(user){ 

        var isEditEnabled = true,
            entityDefinitionCode = options.userAreaCode === 'COF' ? 'COFUSR' : 'COFUSN';

        if (!user) return;

        if (user.accountStatus === 'Deleted') {
            isEditEnabled = false;
            vm.mainForm.formStatus.error('This user cannot be edited because it has been deleted.')
        } else if (user.role.isSuperAdminRole && !currentUser.isSuperAdmin) {
            isEditEnabled = false;
            vm.mainForm.formStatus.error('You need to be in the super admin role to update this user.')
        }

        vm.canUpdate = isEditEnabled && permissionValidationService.canUpdate(entityDefinitionCode);
        vm.canDelete = isEditEnabled && !isCurrentUser && permissionValidationService.canDelete(entityDefinitionCode);
        vm.canResetPassword = isEditEnabled 
            && options.allowPasswordSignIn 
            && options.useEmailAsUsername
            && !isCurrentUser
            && permissionValidationService.hasPermission(entityDefinitionCode + 'RSTPWD');
    }

    function initForm() {
        vm.command = mapUpdateCommand();
        vm.editMode = false;
    }

    function mapUpdateCommand() {

        var command = _.pick(vm.user,
            'userId',
            'firstName',
            'lastName',
            'email',
            'requirePasswordChange',
            'isEmailConfirmed',
            'displayName'
            );

        if (vm.user.accountVerifiedDate) {
            command.isAccountVerified = true;
        }

        if (vm.user.accountStatus === 'Active') {
            command.isActive = true;
        }
        
        if (vm.user.role) {
            command.roleId = vm.user.role.roleId;
        }
        
        command.username = vm.userArea.useEmailAsUsername ? null : vm.user.username;

        return command;
    }

    function redirectToList() {
        $location.path('');
    }

    function setLoadingOn(loadState) {
        vm.globalLoadState.on();
        if (loadState && _.isFunction(loadState.on)) loadState.on();
    }

    function setLoadingOff(loadState) {
        vm.globalLoadState.off();
        if (loadState && _.isFunction(loadState.off)) loadState.off();
    }
}]);