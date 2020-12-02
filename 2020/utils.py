import requests
from os import path
import os

from requests import models

def GetInput(day):
    
    inputFilePath = path.join("input", f"{day}.txt")
    if(path.exists(inputFilePath)):
        inputFile = open(inputFilePath, mode="r")
        data = inputFile.read()
        inputFile.close()
        return data

    file = open("token", mode="r")
    token = file.read()
    file.close()

    auth = {"session": token}
    response = requests.get(url=f"https://adventofcode.com/2020/day/{day}/input", cookies=auth)
    data = response.text

    inputFile = open(inputFilePath, mode="w")
    data = inputFile.write(data)
    inputFile.close()

    return data

def SplitInput(input):
    input.splitlines()