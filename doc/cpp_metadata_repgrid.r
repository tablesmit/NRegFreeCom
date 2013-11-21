# Visual C++ Metadata readout by C# - where to put it
# install.packages("OpenRepGrid")
library(OpenRepGrid)
cpp_metadata_rep <- importTxt("cpp_metadata.repgrid.txt")
bertinCluster(cpp_metadata_rep, color=c("white", "darkred"))