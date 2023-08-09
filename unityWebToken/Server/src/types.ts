export enum MessageType{
    ERROR = 1,
    SUCCESS = 2,
    ENPTY = 3
}

export interface ResponseMSG{
    type:MessageType,
    message: string,
    color?: "#000"
}