//based on oscP5parsing by andreas schlegel

import oscP5.*;
import netP5.*;

String ipNumber = "127.0.0.1";
int receivePort = 9000;
int sendPort = 7110;
OscP5 oscP5;
NetAddress myRemoteLocation;
//---
String[] oscChannelNames = { "isadora/1","isadora/2","isadora/3" };
float[] oscReceiveData = { 0,0,0 };
float[] oscSendData = { 0 };
boolean sendOsc = true;

Button button;

PFont font;
int fontSize = 24;
color receiveOff = color(50,0,0);
color receiveOn = color(255,50,0);
color receiveNow = receiveOff;
int sizeOff = 100;
int sizeOn = 110;
int sizeNow = sizeOff;

int sW = 640;
int sH = 480;
int sD = 500;
int fps = 60;

void setup() {
  size(640, 480, P2D);
  frameRate(fps);
  smooth();
  oscSetup();
  button = new Button(sW/4,sH/2,100,color(0,100,50),fontSize,"Send","ellipse");
  font = createFont("Arial",fontSize);
}

void draw() {
  background(127);
  fill(0);
  textAlign(CENTER);
  textFont(font,fontSize);
  text("OSC Send",sW/4,fontSize*1.5);
  text("OSC Receive",(sW/4)+(sW/2),fontSize*1.5);
  stroke(0);
  strokeWeight(2);
  line(sW/2,0,sW/2,sH);
  rectMode(CENTER);
  noStroke();
  fill(127);
  rect(sW/2,sH/4,100,100);
  rect(sW/2,(sH/2)+(sH/4),100,100);
  button.update();
  oscUse();
  if(oscReceiveData[0]<0.5){
    sizeNow = sizeOn;
    receiveNow = receiveOn;
  }else{
    sizeNow = sizeOff;
    receiveNow = receiveOff;  
  }
  fill(receiveNow);
  rect((sW/4)+(sW/2),sH/2,sizeNow,sizeNow);
  strokeWeight(20);
  stroke(255);
  pushMatrix();
  ellipseMode(CENTER);
  translate(oscReceiveData[0]*sW,abs(sH-(oscReceiveData[1]*sH)),(oscReceiveData[2]*sD)-(sD/2));
  ellipse(0,0,10,10);
  popMatrix();
}
