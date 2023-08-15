import { Router, Request, Response, json } from "express";                                                                      
import axios from "axios";
import {load, CheerioAPI} from "cheerio"
import iconv from 'iconv-lite';
import {Pool} from "./DB";
import {RowDataPacket, } from 'mysql2/promise'
import { MessageType, ResponseMSG } from "./types";

export const lunchRouter = Router();        // 라우터가 만들어짐

lunchRouter.get("/lunch", async (req : Request, res : Response) => {

    let date:string | undefined = req.query.date as string | undefined;

    if (date == undefined)
    {
        date = "20230703";
    }

    let result = await GetDataFromDB(date);  // 디비에 데이터가 있는 지 확인
    if(result != null)
    {
        //DB정보를 보내주면 된다.
        let json = {date, menus: JSON.parse(result[0].menu)};
        //res.render("lunch", json);
        let resPacket : ResponseMSG = {type: MessageType.SUCCESS, message: JSON.stringify(json)};
        res.json(resPacket);
        return;
    }

    const url :string = `http://ggm.hs.kr/lunch.view?date=${date}`; // C#에서 $"" 랑 같은 역할          

    let html = await axios({url, method:"GET", responseType:"arraybuffer"});  // 비동기 함수 Async Task
    
    // 데이터 통신은 모든 데이터를 바이트 스트림으로 통신한다.
    let data : Buffer = html.data;
    let decoded = iconv.decode(data, "euc-kr");
    
    const $ : CheerioAPI = load(decoded);       // HTML 문자열을 Cheerio 에서 로드해서 Cheerio 객체로 만든다.

    let text : string = $(".menuName > span").text();
    let menus : string[] = text.split("\n").map(x => x.replace(/[0-9]+\./g, "")).filter(x => x.length > 0);

    const json = {date, menus};
    //res.render("lunch", json);

    let resPacket : ResponseMSG = {type: MessageType.SUCCESS, message: JSON.stringify(json)};
    res.json(resPacket);

    await Pool.execute("INSERT INTO lunch(date, menu) VALUES(?, ?)", [date, JSON.stringify(menus)]);
});

async function GetDataFromDB(date : string)
{

    //let email = "";
    //let pass = "";
    //const test:string = `SELECT * FROM users WHERE password = '${pass}' AND email = '${email}'`;
    // 위에처럼 하면 해킹될 가능성이 있음. 조심하기
    const spl:string = "SELECT * FROM lunch WHERE date = ?";
    let [row, col] = await Pool.query(spl, [date]);    // escape 처리 특수기호 불가능
    row = row as RowDataPacket[];

    return row.length > 0 ? row : null;
}