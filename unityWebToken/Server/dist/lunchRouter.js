"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.lunchRouter = void 0;
const express_1 = require("express");
const axios_1 = __importDefault(require("axios"));
const cheerio_1 = require("cheerio");
const iconv_lite_1 = __importDefault(require("iconv-lite"));
const DB_1 = require("./DB");
const types_1 = require("./types");
exports.lunchRouter = (0, express_1.Router)(); // 라우터가 만들어짐
exports.lunchRouter.get("/lunch", (req, res) => __awaiter(void 0, void 0, void 0, function* () {
    //let arr: number[] = process.hrtime();   // [초, nano]
    let date = req.query.date;
    if (date == undefined) {
        date = "20230703";
    }
    let result = yield GetDataFromDB(date); // 디비에 데이터가 있는 지 확인
    if (result != null) {
        //DB정보를 보내주면 된다.
        let json = { date, menus: JSON.parse(result[0].menu) };
        //let arr2:number[] = process.hrtime();
        //let secDelta = arr2[0] - arr[0];
        //let nanoDelta = arr[1] - arr[1] + secDelta * 1000000000;        // tjdsmdcmrwjd zhem
        //res.render("lunch", json);
        let resPacket = { type: types_1.MessageType.SUCCESS, message: JSON.stringify(json) };
        res.json(resPacket);
        return;
    }
    const url = `http://ggm.hs.kr/lunch.view?date=${date}`; // C#에서 $"" 랑 같은 역할          
    let html = yield (0, axios_1.default)({ url, method: "GET", responseType: "arraybuffer" }); // 비동기 함수 Async Task
    // 데이터 통신은 모든 데이터를 바이트 스트림으로 통신한다.
    // 리틀 엔디안, 빅엔디안 어쩌고~ 서버 이야기임
    let data = html.data;
    let decoded = iconv_lite_1.default.decode(data, "euc-kr");
    const $ = (0, cheerio_1.load)(decoded); // HTML 문자열을 Cheerio 에서 로드해서 Cheerio 객체로 만든다.
    let text = $(".menuName > span").text();
    let menus = text.split("\n").map(x => x.replace(/[0-9]+\./g, "")).filter(x => x.length > 0);
    const json = { date, menus };
    //res.json({id:1, text:menus});
    // ejs, pug, nunjucks
    //res.render("lunch", json);
    let resPacket = { type: types_1.MessageType.SUCCESS, message: JSON.stringify(json) };
    res.json(resPacket);
    yield DB_1.Pool.execute("INSERT INTO lunch(date, menu) VALUES(?, ?)", [date, JSON.stringify(menus)]);
}));
function GetDataFromDB(date) {
    return __awaiter(this, void 0, void 0, function* () {
        //let email = "";
        //let pass = "";
        //const test:string = `SELECT * FROM users WHERE password = '${pass}' AND email = '${email}'`;
        // 위에처럼 하면 해킹될 가능성이 있음
        const spl = "SELECT * FROM lunch WHERE date = ?";
        let [row, col] = yield DB_1.Pool.query(spl, [date]); // escape 처리 특수기호 불가능
        row = row;
        //console.log(row);
        //console.log(col);
        return row.length > 0 ? row : null;
    });
}
