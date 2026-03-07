export class User {
    id: string = '';
    firstName: string = '';
    email: string = '';
    passwordHash: string = '';
    lastName: string = '';
    refreshToken?: string = '';
    RefreshTokenExpiryTime?: Date
}