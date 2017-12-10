library("plot3D")
distance <- scan("distance-yellow.txt", what=list(data=0))$data
angle <- scan("angle-yellow.txt", what=list(data=0))$data
light <- scan("light-yellow.txt", what=list(data=0))$data
speed <- scan("speed-yellow.txt", what=list(data=0))$data
scatter3D(distance, angle, light, colvar=speed, main="Yellow light", xlab="Distance", ylab="Angle", zlab="Light time", clab="Speed")