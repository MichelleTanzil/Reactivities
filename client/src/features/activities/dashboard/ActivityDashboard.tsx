import React, { useEffect,  useContext } from "react";
import { Grid } from "semantic-ui-react";
import ActivityList from "./ActivityList";
import { observer } from "mobx-react-lite";
import ActivityStore from "../../../app/stores/activityStore";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { RouteComponentProps } from "react-router-dom";

const ActivityDashboard: React.FC<RouteComponentProps> = ({ location }) => {
  const activityStore = useContext(ActivityStore);

  useEffect(() => {
    activityStore.loadActivities();
  }, [activityStore]);
  // Empty array makes sure useEffect only runs once
  // activityStore is a dependency here

  if (activityStore.loadingInitial)
    return <LoadingComponent content="Loading Activities..." />;
  return (
    <Grid>
      <Grid.Column width={10}>
        <ActivityList />
      </Grid.Column>
      <Grid.Column width={6}>
        <h2>Activity filters</h2>
      </Grid.Column>
    </Grid>
  );
};

export default observer(ActivityDashboard);
