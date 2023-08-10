import jwt from 'jsonwebtoken';
import { TokenUser, UserVO } from './types';
import { JWT_SECRET } from './Secret';
import { Request, Response, NextFunction } from 'express';

export const tokenChecker =async (req:Request, res:Response, next:NextFunction) => {
    let token = extractToken(req); 
    if(token != undefined)
    {
        let decodedToken = decodeJWT(token);
        if (decodedToken != null){
            //console.log(token);
            let {id, email, xp, name } = decodeJWT(token) as TokenUser;
            req.user = {id, email, exp:xp, name};  
        }else{
            req.user = null;
        }
    }else {
        req.user = null;
    }

    next();
}

function extractToken(req: Request) 
{
    const PREFIX = "Bearer";
    const auth = req.headers.authorization;
    //프리픽스를 포함한 토큰이면 프리픽스를 제거하고, 그렇지 않다면 그냥 토큰 주고
    const token = auth?.includes(PREFIX) ? auth.split(PREFIX)[1] : auth;
    return token; //만약에 토큰이 존재하지 않았다면 undefined가 리턴될거다.
}

export const createJWT = (userVO: UserVO) => 
{
    const {id, email, name, exp} = userVO;
    const token = jwt.sign( {id,email, name, xp:exp}, JWT_SECRET!, {expiresIn:'7 days'});
    //10h, 100s, 50m 등 가능하다. 또는 아예 ms 단위로 계산해서 넣어줘도 된다. 
    return token;
}

export const decodeJWT = (token: string) => {
    try {
        const decodedToken = jwt.verify(token, JWT_SECRET!); //넘어온 토큰을 디코드함.
        return decodedToken;
    }catch(e)
    {
        //console.log(e);
        return null;
    }
}