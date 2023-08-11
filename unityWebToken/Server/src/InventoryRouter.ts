import { Router, Request, Response, NextFunction } from "express"
import { Pool } from "./DB";
import { RowDataPacket, ResultSetHeader} from 'mysql2/promise'
import { InventoryVO, MessageType, ResponseMSG, UserVO } from "./types";

export const invenRouter = Router();

invenRouter.get("/inven", async (req:Request, res:Response, next:NextFunction) => {
    if(req.user == null)
    {
        res.json({type:MessageType.ERROR, message:"권한이 없습니다."});
        return;
    }

    const user = req.user;

    let sql = "SELECT json FROM inventories WHERE user_id = ?";
    let [rows, col] : [RowDataPacket[], any] = await Pool.query(sql, [user.id]);

    if(rows.length == 0)
    {
        res.json({type:MessageType.EMPTY, message:""});
    }else{
        let json = rows[0].json as string;
        res.json({type:MessageType.SUCCESS, message:json});
    }

});

invenRouter.post("/inven", async (req:Request, res:Response, next:NextFunction) => {
    if(req.user == null)
    {
        res.json({type:MessageType.ERROR, message:"권한이 없습니다."});
        return;
    }

    const user = req.user;
    
    let json = req.body.json;
    let vo : InventoryVO = JSON.parse(json);
    //let vo : InventoryVO = req.body;        // 유니티 버전이 2022 라서

    const sql = `INSERT INTO inventories (user_id, json) VALUES (?, ?) 
                   ON DUPLICATE KEY UPDATE json = VALUES(json)`;
    let [result, info] : [ResultSetHeader, any] = await Pool.execute(sql, [user.id, JSON.stringify(vo)]);
    //result.affectedRows 값이 2면 기존에 존재해서 업데이트 했어, 1이면 없어서 insert했어 

    let msg: ResponseMSG = {type:MessageType.SUCCESS, message:"인벤토리 저장완료"};
    res.json(msg);
});