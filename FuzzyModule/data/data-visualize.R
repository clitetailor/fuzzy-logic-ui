args <- commandArgs(trailingOnly=TRUE)
if (exists(args[2]) && exists(args[3])) {
    lon <- as.integer(args[2])
    lat <- as.integer(args[3])
} else {
    lon <- 40
    lat <- 40
}

library("plot3D")
pdf(paste(toupper(substr(args[1], 1, 1)), tolower(substring(args[1], 2)), " plots(", lon, ", ", lat,").pdf", sep=""))

distance <- scan(paste("distance-", args[1], ".txt", sep=""), what=list(data=0))$data
angle <- scan(paste("angle-", args[1], ".txt", sep=""), what=list(data=0))$data
light <- scan(paste("light-", args[1], ".txt", sep=""), what=list(data=0))$data
speed <- scan(paste("speed-", args[1], ".txt", sep=""), what=list(data=0))$data
scatter3D(distance, angle, light, colvar=speed, 
            main=paste(toupper(substr(args[1], 1, 1)), tolower(substring(args[1], 2)), " light", sep=""), 
            xlab="Distance (m)", ylab="Angle (*)", zlab="Light time (s)", clab="Speed (mph)",
            # theta=lon, phi=lat,
            ticktype="detailed", nticks=5)

library("plot3Drgl")
plotrgl()

while (1) {}