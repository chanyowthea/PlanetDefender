#!/usr/bin/python3
# -*- coding: utf-8 -*- 
 
import chardet
import os

def strJudgeCode(str):
    return chardet.detect(str)

def readFile(path):
    print("read file, file name=%s" % path)
    try:
        f = open(path, 'rb')
        filecontent = f.read()
    finally:
        if f:
            f.close()

    return filecontent

def WriteFile(str, path):
    try:
        print("convert success! write file path=%s" % path)
        f = open(path, 'wb')
        f.write(str)
    finally:
        if f:
            f.close()

def converCode(path):
    file_con = readFile(path)
    result = strJudgeCode(file_con)
    print("convertCode path=%s, encoding=%s" % (path, result['encoding']))
    #print(file_con)
    if result['encoding'] == 'GB2312':
        #os.remove(path)
        srccode = file_con.decode('gbk')
        destcode = srccode.encode('utf-8')    
        WriteFile(destcode, path)

def listDirFile(dir):
    list = os.listdir(dir)
    for line in list:
        filepath = os.path.join(dir, line)
        print("listDirFile filepath=%s" % filepath)
        if os.path.isdir(filepath):
            listDirFile(filepath)
        elif filepath.endswith(".csv"): 
            print(line)
            converCode(filepath)            

if __name__ == '__main__':
    # "D:/_GameDesign/Projects/PlanetDefender/Assets/Resources/CSV"
    listDirFile("../Assets/Resources/CSV")
