import { Router, Request, Response, NextFunction } from "express"
import { Pool } from "./DB";
import { RowDataPacket, ResultSetHeader} from 'mysql2/promise'
import { InventoryVO, MessageType, ResponseMSG, UserVO } from "./types";

export const recordRouter = Router();

// 랭킹 가져오는 것, 데이터 요청
recordRouter.get("/record",async (req:Request, res:Response, next:NextFunction) => {
    const sql = `SELECT u.name, r.* FROM users AS u, ranking AS r WHERE u.id = r.user_id ORDER BY score DESC LIMIT 0,3`;
    // user = u, ranking = r, 로 해서 user의 이름, 랭킹의 모든 것 중에서 유저 아이디와 랭킹 유저 아이디가 같은 것을 가져와 score 기준으로 내림차순이며 상위 3개의 결과를 반환함
    
    //const sql = `SELECT * FROM ranking ORDER BY score DESC LIMIT 0,3`;
    let [row, col] = await Pool.query(sql);
    
    let json = {list:row};
    res.json({type:MessageType.SUCCESS, message:JSON.stringify(json)});
});

// 랭킹 데이터를 제출함. 랭킹 변동됨.
recordRouter.post("/record",async (req:Request, res:Response, next:NextFunction) => {
    if(req.user == null)
    {
        res.json({type:MessageType.ERROR, message:"You do not have permission."});
        return;
    }
    const user = req.user;
    let {score} = req.body;

    const sql = `INSERT INTO ranking(user_id, score, created) VALUES (?, ?, NOW())`;

    await Pool.execute(sql, [user.id, score]);

    res.json({type:MessageType.SUCCESS, message:"Recording complete."})
});