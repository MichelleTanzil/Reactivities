import React, { useContext } from "react";
import { observer } from "mobx-react-lite";
import { Search } from "semantic-ui-react";
import { RootStoreContext } from "../../../app/stores/rootStore";

const ActivitySearchBar = () => {
  const rootStore = useContext(RootStoreContext);
  const { predicate, setPredicate } = rootStore.activityStore;

  const handleSearchChange = () => {};
  return <Search style={{ width: "100%", marginTop: 50 }} />;
};

export default observer(ActivitySearchBar);
