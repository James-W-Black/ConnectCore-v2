namespace ConnectCore_v2.Data
{
    public enum Roles
    {
        Admin, //can do most anything
        createUser, //can create new users
        updateUser, // can edit existing users
        createShifts, //can create shifts
        updateShifts, // can edit shifts
        basic // normal users 
    }
}
