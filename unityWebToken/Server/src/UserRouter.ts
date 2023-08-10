import { Router, Request, Response } from "express";                                                                      
import {Pool} from "./DB";
import {RowDataPacket, ResultSetHeader} from 'mysql2/promise'
import { MessageType, ResponseMSG, UserVO } from "./types";
import { createJWT } from "./MyJWT";

export const userRouter = Router();

userRouter.get("/user", async (req:Request, res:Response) => {
    let resMsg : ResponseMSG = {type:MessageType.EMPTY, message:"로그인이 필요합니다."};
    if(req.user != null)
    {
        console.log(req.user);
        resMsg = {type:MessageType.SUCCESS, message: JSON.stringify(req.user)};
        res.json(resMsg);
    }else {
        res.json(resMsg);
    }
});

userRouter.get("/user/login", async (req:Request, res:Response) => {
    // 이건 구현 안할꺼임
});

userRouter.post("/user/login", async (req:Request, res:Response) => {
    let {email:inputEmail, password} : {email:string, password:string} = req.body;

    const sql = "SELECT * FROM users WHERE email = ? AND password = PASSWORD(?)";
    let [row, col] : [RowDataPacket[], any] = await Pool.query(sql, [inputEmail, password]);
    //console.log(email, password);

    if (row.length == 0){
        res.json({type:MessageType.ERROR, message: "아이디 또는 비밀번호가 올바르지 않습니다."});
        return;
    }

    let {id, email, name, exp} = row[0];

    let user: UserVO = {id, email, name, exp};
    let token = createJWT(user);
    //console.log(user);
    res.json({type:MessageType.SUCCESS, message:JSON.stringify({token, user})});
});

userRouter.get("/user/register", async (req:Request, res:Response) => {
    res.render("register");
});

userRouter.post("/user/register", async (req:Request, res:Response) => {
    console.log(req.body);
    let email : string = req.body.email;
    let password : string = req.body.password;
    let passwordc : string = req.body.passwordc;
    let username : string = req.body.username;

    if (email == "" || password == "" || username == ""){                                           
        let mag:ResponseMSG = {type: MessageType.ERROR, message : "필수값이 비어있습니다"};
        res.json(mag);
        return;
    }
    // email(20*1), name(4*3) => 32바이트
    if (password != passwordc){
        let mag:ResponseMSG = {type: MessageType.ERROR, message : "비밀번호와 확인이 일치하지 않습니다."};
        res.json(mag);
        return;
    }

    const sql : string = "INSERT INTO users (email, password, name) VALUES (?, PASSWORD(?), ?)";
    let [result, info] : [ResultSetHeader, any] = await Pool.execute(sql, [email, password, username]);
    
    if (result.affectedRows != 1){
        let mag : ResponseMSG = {type:MessageType.ERROR, message: "DB에 이상이 있어. ㅁ문의해"};
        res.json(mag);
        return;
    }

    // 데이터 베이스에 값 저장하기

        let mag : ResponseMSG = {type:MessageType.SUCCESS, message: "로그인성공 ㅇ"};
        res.json(mag);
});