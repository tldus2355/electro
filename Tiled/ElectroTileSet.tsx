<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.10" tiledversion="1.11.2" name="ElectroTileSet" tilewidth="32" tileheight="32" tilecount="9" columns="0">
 <grid orientation="orthogonal" width="1" height="1"/>
 <tile id="8" type="Player">
  <properties>
   <property name="voltage" type="int" value="5"/>
  </properties>
  <image source="TiledsetImg/Player.png" width="32" height="32"/>
 </tile>
 <tile id="9" type="Enemy">
  <properties>
   <property name="voltage" type="int" value="5"/>
  </properties>
  <image source="TiledsetImg/Enemy.png" width="32" height="32"/>
 </tile>
 <tile id="10" type="RoadLR">
  <image source="TiledsetImg/-.png" width="32" height="32"/>
 </tile>
 <tile id="11" type="RoadUR">
  <image source="TiledsetImg/ㄴ.png" width="32" height="32"/>
 </tile>
 <tile id="12" type="RoadLUR">
  <image source="TiledsetImg/ㅗ.png" width="32" height="32"/>
 </tile>
 <tile id="13" type="RoadLURD">
  <image source="TiledsetImg/+.png" width="32" height="32"/>
 </tile>
 <tile id="16" type="RoadR">
  <image source="TiledsetImg/half.png" width="32" height="32"/>
 </tile>
 <tile id="17" type="Battery">
  <properties>
   <property name="voltage" type="int" value="3"/>
  </properties>
  <image source="TiledsetImg/Battery.png" width="32" height="32"/>
 </tile>
 <tile id="18" type="Resistance">
  <properties>
   <property name="resistance" type="int" value="1"/>
  </properties>
  <image source="TiledsetImg/Resist.png" width="32" height="32"/>
 </tile>
</tileset>
