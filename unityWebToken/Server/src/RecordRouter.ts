import { Router, Request, Response, NextFunction } from "express"
import { Pool } from "./DB";
import { RowDataPacket, ResultSetHeader} from 'mysql2/promise'
import { InventoryVO, MessageType, ResponseMSG, UserVO } from "./types";

export const recordRouter = Router();

recordRouter.get("/record",async (req:Request, res:Response, next:NextFunction) => {
    const sql = `SELECT u.name, r.* FROM users AS u, ranking AS r WHERE u.id = r.user_id ORDER BY score DESC LIMIT 0,3`;
    //const sql = `SELECT * FROM ranking ORDER BY score DESC LIMIT 0,3`;
    let [row, col] = await Pool.query(sql);
    
    let json = {list:row};
    res.json({type:MessageType.SUCCESS, message:JSON.stringify(json)});
});

recordRouter.post("/record",async (req:Request, res:Response, next:NextFunction) => {
    if(req.user == null)
    {
        res.json({type:MessageType.ERROR, message:"권한이 없습니다."});
        return;
    }
    const user = req.user;
    let {score} = req.body;

    const sql = `INSERT INTO ranking(user_id, score, created) VALUES (?, ?, NOW())`;

    await Pool.execute(sql, [user.id, score]);

    res.json({type:MessageType.SUCCESS, message:"기록완료."})
});