import { UserVO } from "../types";

declare global{
    namespace Express{
        interface Request{
            user: UserVO | null
        }
    }
}